import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { city } from '../Models/city';
import { environment } from './environment';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class CityService {
  private baseUrl = `${environment.baseUrl}/City`;

  constructor(private http: HttpClient, private authService: AuthService) {}

  private getHeaders(): HttpHeaders {
    let token = this.authService.getUserData()?.token || '';
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return headers;
  }

  getAllcities(id: number): Observable<city[]> {
    const headers = this.getHeaders();
    return this.http
      .get<{ $id: number; $values: city[] }>(`${this.baseUrl}/government/${id}`, { headers })
      .pipe(
        map((response) => response.$values)
      );
  }

  getCityById(id: number): Observable<city> {
    const headers = this.getHeaders();
    return this.http.get<city>(`${this.baseUrl}/${id}`, { headers });
  }

  updatecityById(id: number, city: city): Observable<city> {
    const headers = this.getHeaders();
    return this.http.put<city>(`${this.baseUrl}/edit/${id}`, city, { headers });
  }

  addCity(city: city): Observable<any> {
    const headers = this.getHeaders();
    return this.http.post(this.baseUrl, city, { headers });
  }

  deleteCity(id: number): Observable<any> {
    const headers = this.getHeaders();
    return this.http.delete(`${this.baseUrl}/delete/${id}`, { headers });
  }

  changeCityStatus(id: number, status: boolean): Observable<any> {
    const headers = this.getHeaders();
    return this.http.put(`${this.baseUrl}/change-status/${id}?status=${status}`,{},{ headers });
  }
}
