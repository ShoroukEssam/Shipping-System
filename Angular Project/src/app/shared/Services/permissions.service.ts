import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IPermission, IPermissionResponse } from '../Models/Permissions/permission';
import { Observable } from 'rxjs';
import { IRoleWithAllClaims } from '../Models/Permissions/PermissionOnRole';
import { environment } from './environment';
import { AuthService } from './auth.service'; 

@Injectable({
  providedIn: 'root'
})
export class PermissionsService {
  apiUrl: string = `${environment.baseUrl}/Administration`;

  constructor(private http: HttpClient, private authService: AuthService) { }

  private getHeaders(): HttpHeaders {
    let token = this.authService.getUserData()?.token || '';
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return headers;
  }

  getPermissions(): Observable<IPermissionResponse[]> {
    const headers = this.getHeaders();
    return this.http.get<IPermissionResponse[]>(this.apiUrl, { headers });
  }

  getPermissionById(id: string): Observable<IRoleWithAllClaims> {
    const headers = this.getHeaders();
    return this.http.get<IRoleWithAllClaims>(`${this.apiUrl}/GetPermissionsOnRole/${id}`, { headers });
  }

  searchPermissions(query: string): Observable<IPermissionResponse[]> {
    const headers = this.getHeaders();
    return this.http.get<IPermissionResponse[]>(`${this.apiUrl}/${query}`, { headers });
  }

  addPermission(permission: any): Observable<IPermission> {
    const headers = this.getHeaders();
    return this.http.post<IPermission>(this.apiUrl, permission, { headers });
  }

  updatePermission(permission: any): Observable<IPermission> {
    const headers = this.getHeaders();
    return this.http.put<IPermission>(`${this.apiUrl}/${permission.id}`, permission, { headers });
  }

  editPermissionsOnRole(id: string, roleWithClaims: IRoleWithAllClaims): Observable<any> {
    const headers = this.getHeaders().set('Content-Type', 'application/json');
    return this.http.put<any>(`${this.apiUrl}/EditPermissionsOnRole/${id}`, roleWithClaims, { headers });
  }

  deleteRole(id: string): Observable<any> {
    const headers = this.getHeaders();
    return this.http.delete<any>(`${this.apiUrl}/${id}`, { headers });
  }
}
