import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateCompanyOwnerComponent } from './create-company-owner.component';

describe('CreateCompanyOwnerComponent', () => {
  let component: CreateCompanyOwnerComponent;
  let fixture: ComponentFixture<CreateCompanyOwnerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateCompanyOwnerComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CreateCompanyOwnerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
