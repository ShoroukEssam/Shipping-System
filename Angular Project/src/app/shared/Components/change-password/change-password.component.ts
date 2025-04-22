import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PasswordService } from '../../Services/password.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrl: './change-password.component.css'
})
export class ChangePasswordComponent {
  changePasswordForm: FormGroup;

  constructor(private fb: FormBuilder, private passwordService: PasswordService, private router: Router) {
    this.changePasswordForm = this.fb.group({
      oldPassword: ['', Validators.required],
      newPassword: ['', Validators.required],
      confirmNewPassword: ['', Validators.required]
    }, { validator: this.passwordMatchValidator });
  }

  passwordMatchValidator(form: FormGroup) {
    return form.get('newPassword')!.value === form.get('confirmNewPassword')!.value
           ? null : { mismatch: true };
  }

  onSubmit() {
    if (this.changePasswordForm.valid) {
      const passwordDTO = this.changePasswordForm.value;
      this.passwordService.changePassword(passwordDTO).subscribe(
        response => {
          console.log('Password changed successfully:', response);
          this.router.navigate(['/']);
        },
        error => {
          console.error('Error changing password:', error);
          alert(error.error.message || 'خطأ في تغير كلمة المرور');
        }
      );
    }
  }

  goBack() {
    this.router.navigate(['/']);
  }
}
