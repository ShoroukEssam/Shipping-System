import { Component, OnInit } from '@angular/core';
import { AccountService } from './../../Services/account.service';
import { Router } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { loginData } from '../../Models/loginData';
import { ApiService } from '../../Services/api.service';
import { IRoleWithAllClaims } from '../../Models/Permissions/PermissionOnRole';
import { AuthService } from '../../Services/auth.service';

@Component({
  selector: 'app-log-in',
  templateUrl: './log-in.component.html',
  styleUrl: './log-in.component.css'
})
export class LogInComponent{
  isValid=true
  errorMessageService: any;
  showPassword=false

  constructor(private apiService:ApiService,private authService:AuthService,private router:Router){}

  loginForm=new FormGroup({
    email:new FormControl('',[Validators.required,Validators.email]),
    password:new FormControl('',[Validators.required])
  })

  ngOnInit(): void {
    this.loginForm.valueChanges.subscribe(() => {
      this.isValid = true;
    });
  }
  login(){
    const data:loginData={
      email: this.loginForm.value.email!,
      password: this.loginForm.value.password!

    }
    this.apiService.post<any,loginData>('/Account/Login',data).subscribe({
      next: (res) => {
        this.isValid = true;
        this.authService.putUserData(res.user);
        console.log(res.user.roleId)
        this.apiService.get<any>(`/Administration/GetPermissionsOnRole/${res.user.roleId}`).subscribe({

          next: (res:IRoleWithAllClaims) => {
            console.log(res);
            this.authService.putPermission(res);
          }
        })
        this.router.navigate(['/employee/dashboard']);
      },
      error: (err) => {
        console.log(err);
        this.isValid = false;
      },
    })
  }
}
