import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  // Input properties you get from parent component
  // @Input() usersFromHomeComponent: any;

  // Output properties are sent to the parent component
  @Output() cancelRegister = new EventEmitter();
  model: any = {};
  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
  }

  register(){
    this.accountService.register(this.model).subscribe(res => {
      console.log(res);
      this.cancel();
    }, error => {
      console.log(error)
    })
  }

  cancel(){
    // emit value to parent  (home)
    this.cancelRegister.emit(false);
  }

}
