import { AppComponentBase } from '@shared/common/app-component-base';
import { Injector, Input } from '@angular/core';
import { NzModalRef } from 'ng-zorro-antd';

export abstract class ModalComponentBase<T> extends AppComponentBase {
  title = '';
  modalRef: NzModalRef;

  @Input()
  modalInput: T;

  /** 是否为编辑 */
  isEdit: boolean;


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
