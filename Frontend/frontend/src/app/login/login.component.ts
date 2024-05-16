import { Component } from '@angular/core';
import { RouterModule, Router } from '@angular/router';
import { NgModule }      from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { BackendService } from '../backend/backend.service'

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  public username: string = ""

  public errorMessageHidden: boolean = true

  constructor(private service: BackendService, private router: Router) {
  }

  checkLogin(): void {
    if(this.service.checkLogin(this.username)) {
      this.router.navigate(['']);
    } else {
      this.errorMessageHidden = false;
    }
  }
}
