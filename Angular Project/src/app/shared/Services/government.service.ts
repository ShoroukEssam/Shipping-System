import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { Government } from '../Models/government';
import { environment } from './environment';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class GovernmentService {

  private baseUrl = `${environment.baseUrl}/Government`;

  constructor(private http: HttpClient,private authService:AuthService) {}

  private getHeaders(): HttpHeaders {
    const token = this.authService.getUserData()?.token || '';
    return new HttpHeaders().set('Authorization', `Bearer ${token}`);
  }

  getGovernments(): Observable<Government[]> {
    return this.http
      .get<{ $id: string; $values: Government[] }>(this.baseUrl, { headers: this.getHeaders() })
      .pipe(
        map((response) => response.$values)
      );
  }

  getGovernmentById(id: number): Observable<Government> {
    return this.http.get<Government>(`${this.baseUrl}/${id}`, { headers: this.getHeaders() });
  }
}
