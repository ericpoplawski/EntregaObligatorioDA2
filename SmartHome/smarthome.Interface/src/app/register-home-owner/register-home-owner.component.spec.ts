import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegisterHomeOwnerComponent } from './register-home-owner.component';

describe('RegisterHomeOwnerComponent', () => {
  let component: RegisterHomeOwnerComponent;
  let fixture: ComponentFixture<RegisterHomeOwnerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RegisterHomeOwnerComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RegisterHomeOwnerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
