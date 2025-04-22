import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { IClaimsForCheckBox, IRoleWithAllClaims } from '../Models/Permissions/PermissionOnRole';
import { IUserData } from '../Models/IUserData';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private roleClaimsSubject = new BehaviorSubject<IClaimsForCheckBox[]>([]);
  roleClaims$ = this.roleClaimsSubject.asObservable();

  private UserData = new BehaviorSubject<IUserData>({} as IUserData);
  UserData$ = this.UserData.asObservable();

  constructor() {
    this.initializeData();
  }

  private initializeData() {
    const storedUserData = localStorage.getItem('userData');
    const storedRoleClaims = localStorage.getItem('roleClaims');

    if (storedUserData) {
      this.UserData.next(JSON.parse(storedUserData));
    } else {
      this.UserData.next({} as IUserData);
    }

    if (storedRoleClaims) {
      this.roleClaimsSubject.next(JSON.parse(storedRoleClaims));
    } else {
      this.roleClaimsSubject.next([]);
    }
  }

  isAuthenticated(): boolean {
    return !!this.getUserData().userName;
  }

  putUserData(user: IUserData) {
    this.UserData.next(user);
    localStorage.setItem('userData', JSON.stringify(user));
  }

  putPermission(userRole: IRoleWithAllClaims) {
    this.roleClaimsSubject.next(userRole.allRoleCalims.$values);
    localStorage.setItem('roleClaims', JSON.stringify(userRole.allRoleCalims.$values));
  }

  getUserData(): IUserData {
    return this.UserData.getValue(); // Directly return current value synchronously
  }

  getPermission(): IClaimsForCheckBox[] {
    return this.roleClaimsSubject.getValue(); // Directly return current value synchronously
  }

  hasPermission(permission: string): boolean {
    const roleClaims = this.getPermission();
    return roleClaims.some(claim => claim.displayValue === permission && claim.isSelected);
  }

  clearUserData() {
    this.UserData.next({} as IUserData);
    localStorage.removeItem('userData');
  }

  clearPermission() {
    this.roleClaimsSubject.next([]);
    localStorage.removeItem('roleClaims');
  }
}
