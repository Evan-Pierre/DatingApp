import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import {map} from 'rxjs/operators'
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  // data doesnt get destroyd in services until application closes
  baseUrl = 'https://localhost:5001/api/';

                                // type of observable, and we need only 1 because 1 user
  private currentUserSource = new ReplaySubject<User>(1)
  currentUser$ = this.currentUserSource.asObservable();

  // ng g s name --skip-tests      this wil create the service file template
  constructor(private http: HttpClient) { }

  login(model: any){
    return this.http.post(this.baseUrl + 'account/login', model).pipe(
      // everything inside the pipe is RxJS operator
      map((res: User) => {
        const user = res;
        if(user){
          localStorage.setItem('user', JSON.stringify(user))
          this.currentUserSource.next(user);
        }
      })
    )
  }

  register(model:any){
    return this.http.post(this.baseUrl + "account/register", model).pipe(
      map((res: User) => {
        const user = res;
        if(user){
          localStorage.setItem('user', JSON.stringify(user))
          this.currentUserSource.next(user);
        }

        return user;
      })
    )
  }

  setCurrentUser(user:User){
    this.currentUserSource.next(user)
  }

  logout(){
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }
}
