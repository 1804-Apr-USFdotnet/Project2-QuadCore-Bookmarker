import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

import { Collection } from "../collection";
import { CollectionService } from '../collection.service';

@Component({
  selector: 'app-collection-detail',
  templateUrl: './collection-detail.component.html',
  styleUrls: ['./collection-detail.component.css']
})
export class CollectionDetailComponent implements OnInit {
  collection : Collection;

  constructor(private route: ActivatedRoute,
    private collectionService: CollectionService,
    private location: Location) { 

    }

  ngOnInit() {
    this.getCollection();
  }

  getCollection() {
    var id: string = this.route.snapshot.paramMap.get('id');
    this.collectionService.getCollectionById(id).subscribe(
      response => this.collection = response,
      errors => console.log(errors)
    );
  }

  goBack() : void {
    this.location.back();
  }
}
