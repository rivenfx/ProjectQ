import { Component, Injector, OnInit } from '@angular/core';
import { ModalComponentBase } from '@shared/common';

@Component({
  selector: 'create-or-edit-user',
  templateUrl: './create-or-edit-user.component.html',
  styleUrls: ['./create-or-edit-user.component.less']
})
export class CreateOrEditUserComponent extends ModalComponentBase<string>
  implements OnInit {

  constructor(
    injector: Injector,
  ) {
    super(injector);
  }

  ngOnInit(): void {
  }

  submitForm(event?: any) {
  }
}
