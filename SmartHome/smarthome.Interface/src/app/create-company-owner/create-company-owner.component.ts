import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthenticationService } from '../services/authentication.service';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-create-company-owner',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './create-company-owner.component.html',
  styleUrls: ['./create-company-owner.component.css']
})
export class CreateCompanyOwnerComponent {
  ownerForm: FormGroup;
  errorMessage: string | null = null;

  constructor(
    private fb: FormBuilder,
    private authService: AuthenticationService,
    private router: Router
  ) {
    this.ownerForm = this.fb.group({
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  onSubmit(): void {
    if (this.ownerForm.valid) {
      const ownerData = this.ownerForm.value;
      this.authService.registerCompanyOwner(ownerData).subscribe(
        response => {
          this.errorMessage = null;
          this.router.navigate(['/admin-dashboard']);
        },
        error => {
          this.errorMessage = error;
          console.error('Error creating admin account:', error);
        }
      );
    }
  }
}
