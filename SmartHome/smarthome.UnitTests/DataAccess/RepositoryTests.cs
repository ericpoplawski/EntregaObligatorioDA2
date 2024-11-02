using Domain;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using smarthome.DataAccess;

[TestClass]
public class RepositoryTests
{
    private ApplicationDbContext _context;
    private Repository<User> _repository;

    [TestInitialize]
    public void TestInitialize()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new Repository<User>(_context);
    }

    [TestMethod]
    public void Add_ShouldAddEntityAndSaveChanges()
    {
        var role = new Role
        {
            Id = "1",
            RoleName = "Admin"
        };
        
        var entity = new User
        {
            Email = "test@example.com",
            FirstName = "Test",
            FullName = "Test User",
            LastName = "User",
            Password = "TestPassword",
            Roles = new List<Role>()
            {
                role
            }
        };
        
        _repository.Add(entity);
        
        _context.Users.Should().Contain(entity);
    }
    
    [TestMethod]
    public void Exists_WhenEntityExists_ShouldReturnTrue()
    {
        var role = new Role
        {
            Id = "1",
            RoleName = "Admin"
        };
        var entity = new User
        {
            Email = "test@example.com",
            FirstName = "Test",
            FullName = "Test User",
            LastName = "User",
            Password = "TestPassword",
            Roles = new List<Role> { role }
        };
        _repository.Add(entity);
        
        var exists = _repository.Exists(u => u.Email == entity.Email);
        
        exists.Should().BeTrue();
    }

    
    [TestMethod]
    public void Get_WhenEntityExistsWithIncludes_ShouldReturnEntityWithIncludes()
    {
        var role = new Role
        {
            Id = "1",
            RoleName = "Admin"
        };
        _context.Roles.Add(role);
        _context.SaveChanges();

        var entity = new User
        {
            Email = "test@example.com",
            FirstName = "Test",
            FullName = "Test User",
            LastName = "User",
            Password = "TestPassword",
            Roles = new List<Role>
            {
                role
            }
        };
        _repository.Add(entity);

        var retrievedEntity = _repository.Get(u => u.Email == entity.Email, u => u.Roles);

        retrievedEntity.Should().BeEquivalentTo(entity, options => options.Including(u => u.Roles));
        retrievedEntity.Roles[0].Id.Should().Be(role.Id);
    }
    
    [TestMethod]
    public void Get_WhenEntityDoesNotExist_ShouldThrowException()
    {
        var nonExistentEmail = "nonexistent@example.com";
        
        Action act = () => _repository.Get(u => u.Email == nonExistentEmail);
        
        act.Should().Throw<Exception>().WithMessage("Entity not found");
    }

    [TestMethod]
    public void GetAllWithPagination_WhenEntitiesExistWithPredicate_ShouldReturnEntitiesMatchingPredicate()
    {
        var role1 = new Role
        {
            Id = "1",
            RoleName = "Admin"
        };
        var role2 = new Role
        {
            Id = "2",
            RoleName = "User"
        };
        _context.Roles.Add(role1);
        _context.Roles.Add(role2);
        _context.SaveChanges();

        var entity1 = new User
        {
            Email = "test1@example.com",
            FirstName = "Test1",
            FullName = "Test User1",
            LastName = "User1",
            Password = "TestPassword1",
            Roles = new List<Role>
            {
                role1
            }
        };
        var entity2 = new User
        {
            Email = "test2@example.com",
            FirstName = "Test2",
            FullName = "Test User2",
            LastName = "User2",
            Password = "TestPassword2",
            Roles = new List<Role>
            {
                role2
            }
        };
        _repository.Add(entity1);
        _repository.Add(entity2);

        var retrievedEntities = _repository.GetAllWithPagination(1, 2, u => u.Roles[0].Id == role1.Id, u => u.Roles);

        retrievedEntities.Count.Should().Be(1);
        retrievedEntities[0].Should().BeEquivalentTo(entity1, options => options.Including(u => u.Roles));
        retrievedEntities[0].Roles[0].Should().BeEquivalentTo(role1);
    }

    [TestMethod]
    public void GetAll_WhenEntitiesExistWithPredicateAndIncludes_ShouldReturnEntitiesMatchingPredicateAndIncludes()
    {
        var role1 = new Role
        {
            Id = "1",
            RoleName = "Admin"
        };
        var role2 = new Role
        {
            Id = "2",
            RoleName = "User"
        };
        _context.Roles.Add(role1);
        _context.Roles.Add(role2);
        _context.SaveChanges();

        var entity1 = new User
        {
            Email = "test1@example.com",
            FirstName = "Test1",
            FullName = "Test User1",
            LastName = "User1",
            Password = "TestPassword1",
            Roles = new List<Role>
            {
                role1
            }
        };
        var entity2 = new User
        {
            Email = "test2@example.com",
            FirstName = "Test2",
            FullName = "Test User2",
            LastName = "User2",
            Password = "TestPassword2",
            Roles = new List<Role>
            {
                role2
            }
        };
        _repository.Add(entity1);
        _repository.Add(entity2);

        var retrievedEntities = _repository.GetAll(u => u.Roles[0].Id == role1.Id, u => u.Roles);

        retrievedEntities.Count.Should().Be(1);
        retrievedEntities[0].Should().BeEquivalentTo(entity1, options => options.Including(u => u.Roles));
        retrievedEntities[0].Roles[0].Should().BeEquivalentTo(role1);
    }
    
    [TestMethod]
    public void Update_ShouldUpdateEntity()
    {
        var role = new Role
        {
            Id = "1",
            RoleName = "Admin"
        };
        
        var entity = new User
        {
            Email = "test@example.com",
            FirstName = "Test",
            FullName = "Test User",
            LastName = "User",
            Password = "TestPassword",
            Roles = new List<Role>
            {
                role
            }
        };
        _repository.Add(entity);
        
        entity.Email = "updated@example.com";
        _repository.Update(entity);
        
        var updatedEntity = _repository.Get(u => u.Email == entity.Email);
        updatedEntity.Should().BeEquivalentTo(entity);
    }
    
    [TestMethod]
    public void Remove_ShouldRemoveEntity()
    {
        var role = new Role
        {
            Id = "1",
            RoleName = "Admin"
        };
        
        var entity = new User
        {
            Email = "test@example.com",
            FirstName = "Test",
            FullName = "Test User",
            LastName = "User",
            Password = "TestPassword",
            Roles = new List<Role>()
            {
                role
            }
        };
        _repository.Add(entity);
    
        _repository.Remove(entity);
    
        var exists = _repository.Exists(u => u.Email == entity.Email);
        exists.Should().BeFalse();
    }

    
    [TestCleanup]
    public void TestCleanup()
    {
        _context.Database.EnsureDeleted();
    }
}