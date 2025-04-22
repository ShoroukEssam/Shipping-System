import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { AuthService } from '../../Services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit , OnDestroy {
  isloggedin: boolean = false;
  userDataSubscription: Subscription | undefined;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    // Initial setup
    this.isloggedin = !!this.authService.getUserData().userName;

    // Subscribe to userData changes
    this.userDataSubscription = this.authService.UserData$.subscribe(userData => {
      this.isloggedin = !!userData.userName;
    });
  }

  ngOnDestroy(): void {
    if (this.userDataSubscription) {
      this.userDataSubscription.unsubscribe();
    }
  }
  start(){
    if(this.isloggedin){
      this.router.navigate(['shared/dashboard']);
    }else{
      this.router.navigate(['shared/login']);
    }
  }
}
