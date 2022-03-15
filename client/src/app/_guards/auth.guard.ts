import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AccountService } from '../_services/account.service';


// ng g guard name --skip-tests
@Injectable({
  providedIn: 'root'
})

//can have multiple guards, if single one fails, routing is stopped
// auto subscribes to observables
// remember to add guard to route
export class AuthGuard implements CanActivate {

  constructor(private accountService: AccountService, private toastr : ToastrService){}
  canActivate(): Observable<boolean> {
    return this.accountService.currentUser$.pipe(
      map(user => {
        if(user) return true

        this.toastr.error('You shall not pass!');
      })
    );
  }
  
}
