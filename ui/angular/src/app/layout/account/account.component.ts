import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'layout-account',
  templateUrl: './account.component.html',
  styleUrls:['./account.component.less']
})
export class LayoutAccountComponent implements OnInit {

  links = [
    {
      title: '帮助',
      href: '',
    },
    {
      title: '隐私',
      href: '',
    },
    {
      title: '条款',
      href: '',
    },
  ];

  constructor() { }

  ngOnInit(): void {
  }

}
