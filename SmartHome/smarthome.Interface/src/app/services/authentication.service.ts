import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { catchError, Observable, tap, throwError } from 'rxjs';
import SessionResponse from './session/models/SessionResponse';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private apiUrl = 'https://localhost:44375/api';
  private isLogged = false;

  constructor(private http: HttpClient) {}

  register(data: { firstName: string; lastName: string; email: string; password: string; profilePicture: string }): Observable<any> {
    const headers = this.getAuthHeaders();
    return this.http.post(`${this.apiUrl}/homeOwners`, data, { headers }).pipe(
      catchError(this.handleError)
    );
  }

  login(data: { email: string; password: string }): Observable<any> {
    return this.http.post<SessionResponse>(`${this.apiUrl}/sessions`, data).pipe(
      tap(response => {
        localStorage.setItem('token', response.token);
        this.isLogged = true;
      }),
      catchError(error => {
        localStorage.removeItem('token');
        this.isLogged = false;
        return this.handleError(error);
      })
    );
  }

  registerAdmin(data: { firstName: string; lastName: string; email: string; password: string }): Observable<any> {
    const headers = this.getAuthHeaders();
    return this.http.post(`${this.apiUrl}/administrators`, data, { headers }).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'Ocurrió un error inesperado';

    if (error.error instanceof ErrorEvent) {
      errorMessage = `Error: ${error.error.message}`;
    } else {
      errorMessage = error.error?.message || `Error ${error.status}: ${error.message}`;
    }

    return throwError(errorMessage);
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('roles');
    this.isLogged = false;
  }

  hasRole(role: string): boolean {
    const token = localStorage.getItem('token');
    if (token) {
      try {
        const payload = JSON.parse(atob(token.split('.')[1]));
        return payload.roles && Array.isArray(payload.roles) && payload.roles.includes(role);
      } catch (e) {
        console.error('Error decodificando el token', e);
        return false;
      }
    }
    return false;
  }

  getRoles(): string[] {
    const token = localStorage.getItem('token');
    if (token) {
      try {
        const payload = JSON.parse(atob(token.split('.')[1]));
        return payload.roles && Array.isArray(payload.roles) ? payload.roles : [];
      } catch (e) {
        console.error('Error decodificando el token', e);
        return [];
      }
    }
    return [];
  }

  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    return new HttpHeaders().set('Authorization', `Bearer ${token}`);
  }

  registerCompanyOwner(data: { firstName: string; lastName: string; email: string; password: string }): Observable<any> {
    const headers = this.getAuthHeaders();
    return this.http.post(`${this.apiUrl}/companyOwners`, data, { headers }).pipe(
      catchError(this.handleError)
    );
  }

  getUsers(): Observable<any[]> {
    const headers = this.getAuthHeaders();
    return this.http.get<any[]>(`${this.apiUrl}/users`, { headers }).pipe(
      catchError(this.handleError)
    );
  }




}
