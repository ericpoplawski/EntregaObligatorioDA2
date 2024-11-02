import { Routes } from '@angular/router';
import { RegisterComponent } from './register-home-owner/register-home-owner.component';
import {LoginComponent} from "./login/login.component";
import {LandingPageComponent} from "./landing-page/landing-page.component";
import {AdminDashboardComponent} from "./admin-dashboard/admin-dashboard.component";
import { CreateAdminComponent } from './create-admin/create-admin.component';
import { CreateCompanyOwnerComponent } from './create-company-owner/create-company-owner.component';
import { ListUsersComponent } from './list-users/list-users.component';


export const routes: Routes = [
  { path: 'register-home-owner', component: RegisterComponent },
  { path: 'login', component: LoginComponent },
  { path: 'admin-dashboard', component: AdminDashboardComponent },
  { path: 'landing-page', component: LandingPageComponent },
  { path: 'create-admin', component: CreateAdminComponent},
  { path: 'create-company-owner', component: CreateCompanyOwnerComponent },
  { path: 'list-users', component: ListUsersComponent },
  { path: '', redirectTo: '/landing-page', pathMatch: 'full' },
  // otras rutas
];
