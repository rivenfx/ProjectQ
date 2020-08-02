import { ListViewComponentBase } from '@shared/common/list-view-component-base';
import { NzModalRef } from 'ng-zorro-antd';
import { Injector } from '@angular/core';

export abstract class ModalListViewComponentBase<T> extends ListViewComponentBase<T> {
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
