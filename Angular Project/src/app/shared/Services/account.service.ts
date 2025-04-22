import { ApiService } from './api.service';
import { Injectable ,OnInit} from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { loginData } from '../Models/loginData';
import { environment } from './environment';
import { Router } from '@angular/router';
import { IchangePassword } from '../Models/IchangePassword';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class AccountService{

  baseUrl: string = `${environment.baseUrl}`;
  token:string =  '';
  constructor(private apiService:ApiService,private AuthService:AuthService,private http: HttpClient,private router:Router) {
  }

  private getHeaders(): HttpHeaders {
    let token  = this.AuthService.getUserData()?.token || '';
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return headers;
  }

  Login_Account(formData: loginData){
    this.apiService.post<any,loginData>('/Account/Login',formData)
  }

  LogOut_Account( ): Observable<any> {
    return this.http.post(`${this.baseUrl}/Account/Logout`,{},{headers: this.getHeaders()});
  }

  ChangePassword_Account( body: IchangePassword ): Observable<any> {
    return this.http.post(`${this.baseUrl}/Account/changePassword`, body,{headers: this.getHeaders()});
  }

}
