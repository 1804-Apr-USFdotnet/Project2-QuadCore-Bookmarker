import { Component, OnInit } from '@angular/core';
import { Collection } from '../collection';
import { CollectionService } from '../collection.service'

@Component({
  selector: 'app-collection',
  templateUrl: './collection.component.html',
  styleUrls: ['./collection.component.css']
})
export class CollectionComponent implements OnInit {

  collections: Collection[];

  constructor(private collectionSvc: CollectionService) { }

  ngOnInit() {
    this.getCollections();
  }

  getCollections(): void {

  }

}
