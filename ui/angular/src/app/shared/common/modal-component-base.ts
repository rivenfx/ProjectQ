import { AppComponentBase } from '@shared/common/app-component-base';
import { Injector } from '@angular/core';
import { NzModalRef } from 'ng-zorro-antd';
import { IModalComponent } from './i-modal-component';

export abstract class ModalComponentBase extends AppComponentBase
  implements IModalComponent {
  title = '';
  modalRef: NzModalRef;

  constructor(injector: Injector) {
    super(injector);
    this.modalRef = injector.get(NzModalRef);
  }


  success(res: boolean | any = true) {
    this.modalRef.close(res);
  }

  close(res: boolean | any = false) {
    this.success(res);
  }

  abstract submitForm(event?: any);
}
