import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { AuthenticationService } from '../services/authentication.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-create-admin',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './create-admin.component.html',
  styleUrls: ['./create-admin.component.css']
})
export class CreateAdminComponent {
  adminForm: FormGroup;
  errorMessage: string | null = null;

  constructor(private fb: FormBuilder, private authService: AuthenticationService, private router: Router) {
    this.adminForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  onSubmit(): void {
    if (this.adminForm.valid) {
      const adminData = this.adminForm.value;
      this.authService.registerAdmin(adminData).subscribe(
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
