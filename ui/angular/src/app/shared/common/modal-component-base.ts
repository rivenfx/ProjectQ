import { AppComponentBase } from '@shared/common/app-component-base';
import { Injector } from '@angular/core';
import { NzModalRef } from 'ng-zorro-antd';

export abstract class ModalComponentBase extends AppComponentBase {
  title = '';
  modalRef: NzModalRef;

  constructor(injector: Injector) {
    super(injector);
    try {
      this.modalRef = injector.get(NzModalRef);
    } catch (e) {

    }
  }


  success(res: boolean | any = true) {
    if (this.modalRef) {
      this.modalRef.close(res);
    }
  }

  close(res: boolean | any = false) {
    this.success(res);
  }

  abstract submitForm(event?: any);
}
