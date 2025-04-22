import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable,OnInit } from '@angular/core';
import { environment } from './environment';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})

export class ApiService {
  private baseUrl = environment.baseUrl;
  constructor(private http: HttpClient,private AuthService:AuthService) {}

  private getHeaders(): HttpHeaders {
    let token  = this.AuthService.getUserData()?.token || '';
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return headers;
  }

  get<T>(url: string) {
    return this.http.get<T>(this.baseUrl + url,{headers: this.getHeaders()});
  }

  getPagination<T>(url: string ,page: number = 0, pageSize: number = 0) {
    return this.http.get<T>(`${this.baseUrl}?page=${page}&pageSize=${pageSize}`,{headers: this.getHeaders()});
  }

  post<T, R>(url: string, data: R) {
    return this.http.post<T>(this.baseUrl+ url, data,{headers: this.getHeaders()});
  }

  put<T, R>(url: string, data: R) {
    return this.http.put<T>(this.baseUrl + url, data,{headers: this.getHeaders()});
  }

  delete<T>(url: string) {
      return this.http.delete<T>(this.baseUrl + url,{headers: this.getHeaders()});
  }
}
