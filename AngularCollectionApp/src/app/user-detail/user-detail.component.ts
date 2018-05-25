import { Component, OnInit, Input } from '@angular/core';
import { User } from '../user'
import { ActivatedRoute } from '@angular/router'
import { Location } from '@angular/common'
import { UserService } from '../user.service'

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.css']
})
export class UserDetailComponent implements OnInit {

  constructor(
    private route: ActivatedRoute,
    private userService: UserService,
    private location: Location
  ) { }

  @Input() user: User;

  ngOnInit() {
    this.getUser();
  }

  getUser(): void {
    const name = this.route.snapshot.paramMap.get('username');
    var obUsers = this.userService.getUsers().subscribe(
      (users) => { this.user = users.find(user => user.username === name); }
    );
  }

  goBack(): void {
    this.location.back();
  }

}
