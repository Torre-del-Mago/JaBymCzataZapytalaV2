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

  async checkLogin(): void {
    this.service.checkLogin(this.username).subscribe(
      {
        next: (x: any) => {this.service.setUser(this.username);
          this.router.navigate(['']);
        }, 
        error: (x: any) => {this.errorMessageHidden = false}
      }
    )
  }
}
