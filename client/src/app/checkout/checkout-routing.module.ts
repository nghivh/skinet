import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { CheckoutComponent } from './checkout.component';
import { CheckoutSuccessComponent } from './checkout-success/checkout-success.component';

const route: Routes = [
  {path: '', component: CheckoutComponent},
  {path: 'success', component: CheckoutSuccessComponent}
]

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(route)
  ],
  exports: [
    RouterModule
  ]
})
export class CheckoutRoutingModule { }
