import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-account-layout',
  templateUrl: './account-layout.component.html',
  styleUrls:['./account-layout.component.less']
})
export class AccountLayoutComponent implements OnInit {

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
