import { Component, OnInit } from '@angular/core';
import { User } from '../user'
import { UserService } from '../user.service'

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {

  users: User[];

  constructor(private userSvc: UserService) { }

  ngOnInit() {
    this.getUsers();
  }

  getUsers():void {
    this.userSvc.getUsers()
      .subscribe(
        response => this.users = response,
        errors => console.log(errors)
      );
  }
}
