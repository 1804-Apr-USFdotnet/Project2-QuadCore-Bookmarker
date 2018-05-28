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
  sort: string = "name:asc";
  sortOptions: Object[] = [
    { name: "Name asc.", value: "name:asc" },
    { name: "Name desc.", value: "name:desc" }
  ];

  constructor(private userSvc: UserService) { }

  ngOnInit() {
    this.getUsers();
  }

  getUsers():void {
    this.userSvc.getUsers(null, this.sort)
      .subscribe(
        response => this.users = response,
        errors => console.log(errors)
      );
  }
}
