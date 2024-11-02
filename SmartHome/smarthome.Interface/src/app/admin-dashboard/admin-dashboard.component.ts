import { Component } from '@angular/core';
import { RouterModule, Router } from "@angular/router";

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.css']
})
export class AdminDashboardComponent {

  constructor(private router: Router) {}

  switchToHomeOwner(): void {
    // Redirigir al Home Owner Dashboard o actualizar el rol
    this.router.navigate(['/home-owner-dashboard']);
  }
}
