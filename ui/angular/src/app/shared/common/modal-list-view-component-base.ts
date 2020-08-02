import { ListViewComponentBase } from '@shared/common/list-view-component-base';
import { NzModalRef } from 'ng-zorro-antd';
import { Injector } from '@angular/core';

export abstract class ModalListViewComponentBase<T> extends ListViewComponentBase<T> {
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
