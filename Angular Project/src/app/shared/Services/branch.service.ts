import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { getAllBranch, Branch } from '../Models/branch'; 
import { environment } from './environment';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class BranchService {
  private baseUrl = `${environment.baseUrl}/Branch`;

  constructor(private http: HttpClient, private authService: AuthService) {}

  private getHeaders(): HttpHeaders {
    let token = this.authService.getUserData()?.token || '';
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return headers;
  }

  getBranches(): Observable<getAllBranch[]> {
    const headers = this.getHeaders();
    return this.http
      .get<{ $id: string; $values: getAllBranch[] }>(this.baseUrl, { headers })
      .pipe(
        map((response) => response.$values) // Extract the values array
      );
  }

  getBranchById(id: number): Observable<getAllBranch> {
    const headers = this.getHeaders();
    return this.http.get<getAllBranch>(`${this.baseUrl}/${id}`, { headers });
  }

  updateBranchById(id: number, branch: Branch): Observable<Branch> {
    const headers = this.getHeaders();
    return this.http.put<Branch>(`${this.baseUrl}/${id}`, branch, { headers });
  }

  addBranch(branch: any): Observable<any> {
    const headers = this.getHeaders();
    return this.http.post(this.baseUrl, branch, { headers });
  }

  deleteBranch(id: number): Observable<any> {
    const headers = this.getHeaders();
    return this.http.delete(`${this.baseUrl}/${id}`, { headers });
  }
}
