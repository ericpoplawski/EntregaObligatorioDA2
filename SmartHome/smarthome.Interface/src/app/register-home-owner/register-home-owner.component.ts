import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { AuthenticationService } from '../services/authentication.service';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';

@Component({
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, HttpClientModule],
  selector: 'app-register',
  templateUrl: './register-home-owner.component.html',
  styleUrls: ['./register-home-owner.component.css']
})
export class RegisterComponent {
  registerForm: FormGroup;
  errorMessage: string | null = null;
  successMessage: string | null = null;

  constructor(private fb: FormBuilder, private authService: AuthenticationService) {
    this.registerForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      profilePicture: [''],
    });
  }

  onSubmit() {
    if (this.registerForm.valid) {
      const { firstName, lastName, email, password, profilePicture } = this.registerForm.value;
      this.authService.register({ firstName, lastName, email, password, profilePicture }).subscribe(
          response => {
            this.successMessage = 'Registration successful';
            this.errorMessage = null;
          },
          error => {
            this.successMessage = null;
            this.errorMessage = error;
          }
      );
    }
  }
}
