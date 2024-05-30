import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {SearchComponent} from "./search/search.component"
import {LoginComponent} from "./login/login.component"
import {DetailComponent} from "./detail/detail.component"
import {ReserveComponent} from "./reserve/reserve.component"

const routes: Routes = [
  {path: '', component: SearchComponent},
  {path: 'login', component: LoginComponent},
  {path: 'detail', component: DetailComponent},
  {path: 'reserve', component: ReserveComponent},
  {path: '**', redirectTo: ''}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
