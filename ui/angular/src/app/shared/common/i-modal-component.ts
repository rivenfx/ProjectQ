import { NzModalRef } from 'ng-zorro-antd';
import { Injector } from '@angular/core';

export interface IModalComponent {
  title: string;
  modalRef: NzModalRef;


  success: (res: boolean | any) => void;

  close: (res: boolean | any) => void;

  submitForm: (event?: any) => void;
}
