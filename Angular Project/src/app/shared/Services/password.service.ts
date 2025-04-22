import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from './environment';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class PasswordService {
  private apiUrl = `${environment.baseUrl}/Account/changePassword`;

  constructor(private http: HttpClient, private authService: AuthService) {}

  private getHeaders(): HttpHeaders {
    let token = this.authService.getUserData()?.token || '';
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return headers;
  }

  changePassword(passwordDTO: any): Observable<any> {
    const headers = this.getHeaders();
    return this.http.post(this.apiUrl, passwordDTO, { headers });
  }
}
