import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {SearchComponent} from "./search/search.component"
import {LoginComponent} from "./login/login.component"
import {DetailComponent} from "./detail/detail.component"

const routes: Routes = [
  {path: '', component: SearchComponent},
  {path: 'login', component: LoginComponent},
  {path: 'detail', component: DetailComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
