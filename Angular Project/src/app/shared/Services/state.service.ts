import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { IState } from '../Models/IState';
import { environment } from './environment';
import { AuthService } from './auth.service'; // Assuming AuthService handles token retrieval

@Injectable({
  providedIn: 'root'
})
export class StateService {

  private baseUrl = `${environment.baseUrl}/Government`;

  constructor(private http: HttpClient, private authService: AuthService) {}

  private getHeaders(): HttpHeaders {
    const token = this.authService.getUserData()?.token || '';
    return new HttpHeaders().set('Authorization', `Bearer ${token}`);
  }

  getGovernments(): Observable<IState[]> {
    const url = `${this.baseUrl}`;
    const headers = this.getHeaders();
    return this.http.get<any>(url, { headers }).pipe(
      map(response => response.$values as IState[])
    );
  }

  getGovernmentById(id: number): Observable<IState> {
    const url = `${this.baseUrl}/${id}`;
    const headers = this.getHeaders();
    return this.http.get<IState>(url, { headers });
  }

  addNewGovernment(state: IState): Observable<IState> {
    const url = `${this.baseUrl}/add`;
    const headers = this.getHeaders();
    return this.http.post<IState>(url, state, { headers });
  }

  editGovernment(id: number, state: IState): Observable<IState> {
    const url = `${this.baseUrl}/edit/${id}`;
    const headers = this.getHeaders();
    return this.http.put<IState>(url, state, { headers });
  }

  deleteGovernment(id: number): Observable<any> {
    const url = `${this.baseUrl}/delete/${id}`;
    const headers = this.getHeaders();
    return this.http.delete(url, { headers });
  }
}
