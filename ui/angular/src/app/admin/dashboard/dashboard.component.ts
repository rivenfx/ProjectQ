import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.less']
})
export class DashboardComponent implements OnInit {

  selectVals = [
    'user.query'
  ];

  constructor() { }

  ngOnInit(): void {
  }

}
