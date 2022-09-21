import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-pager',
  templateUrl: './pager.component.html',
  styleUrls: ['./pager.component.sass']
})
export class PagerComponent implements OnInit {

@Input() totalCount:number
@Input() pageSize:number
@Output() pageChanged=new EventEmitter<number>()


  constructor() { }

  ngOnInit(): void {
  }

  onPagerChange(event)
  {
    debugger
    this.pageChanged.emit(event)
  }
}
