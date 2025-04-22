import { Component, OnInit, OnDestroy } from '@angular/core';
import { AccountService } from '../../Services/account.service';
import { Router } from '@angular/router';
import { AuthService } from './../../Services/auth.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-aside',
  templateUrl: './aside.component.html',
  styleUrls: ['./aside.component.css']
})
export class AsideComponent implements OnInit, OnDestroy {
  role: string = '';
  user: string = '';
  isloggedin: boolean = false;
  userDataSubscription: Subscription | undefined;
  roleClaimsSubscription: Subscription | undefined;

  constructor(
    private accountService: AccountService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    // Initial setup
    this.isloggedin = !!this.authService.getUserData().userName;
    this.role = this.authService.getUserData().role;
    this.user = this.authService.getUserData().userName;

    // Subscribe to userData changes
    this.userDataSubscription = this.authService.UserData$.subscribe(userData => {
      this.isloggedin = !!userData.userName;
      this.user = userData.userName || '';
      this.role = userData.role || '';
    });

    // Subscribe to roleClaims changes
    this.roleClaimsSubscription = this.authService.roleClaims$.subscribe(roleClaims => {
      // Handle role claims update if needed
      // This example assumes you're updating 'role' based on role claims
    });
  }

  ngOnDestroy(): void {
    // Clean up subscriptions to avoid memory leaks
    if (this.userDataSubscription) {
      this.userDataSubscription.unsubscribe();
    }
    if (this.roleClaimsSubscription) {
      this.roleClaimsSubscription.unsubscribe();
    }
  }

  dropdowns = {
    usersDropdown: false,
    regionsDropdown: false,
    settingsDropdown: false,
    userSettingsDropdown: false
  };

  toggleDropdown(dropdown: keyof typeof this.dropdowns) {
    this.dropdowns[dropdown] = !this.dropdowns[dropdown];
  }
  resetDropdowns() {
    this.dropdowns = {
      usersDropdown: false,
      regionsDropdown: false,
      settingsDropdown: false,
      userSettingsDropdown: false
    };
  }

  logout() {
    this.accountService.LogOut_Account();
    this.authService.clearPermission();
    this.user = '';
    this.role = '';
    this.resetDropdowns();
    this.authService.clearUserData();
    this.router.navigate(['shared/login']);
  }
}
