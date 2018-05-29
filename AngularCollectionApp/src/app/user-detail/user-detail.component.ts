import { Component, OnInit, Input } from '@angular/core';
import { User } from '../user'
import { ActivatedRoute } from '@angular/router'
import { Location } from '@angular/common'
import { UserService } from '../user.service'
import { Collection } from '../collection';
import { CollectionService } from '../collection.service';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.css']
})
export class UserDetailComponent implements OnInit {

  constructor(
    private route: ActivatedRoute,
    private userService: UserService,
    private collectionService: CollectionService,
    private location: Location
  ) { }

  user: User;
  myCollections: Collection[];
  collectionGroups: Object[];

  ngOnInit() {
    this.getUser();
    this.initCollections();
  }

  getUser(): void {
    const id = this.route.snapshot.paramMap.get('id');
    this.userService.getUserById(id).subscribe(
      response => this.user = response,
      errors => console.log(errors)
    );
  }

  initCollections(): void {
    const id = this.route.snapshot.paramMap.get('id');
    this.collectionService.getCollectionsByUserId(id).subscribe(
      //response => this.myCollections = response,
      response => this.makeCollectionGroups(response),
      errors => console.log(errors)
    );
  }

  makeCollectionGroups(collectionList: Collection[]) {
    this.collectionGroups = [];
    var group: object[] = [];
    collectionList.forEach(eachObject => {
      if (!eachObject.private) {
        group.push(eachObject);
        if (group.length === 3) {
          this.collectionGroups.push(group);
          group = [];
        }
      }
    });
    this.collectionGroups.push(group);
  }

  goBack(): void {
    this.location.back();
  }

}
