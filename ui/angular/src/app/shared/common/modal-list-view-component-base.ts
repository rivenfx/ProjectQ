import { ListViewComponentBase } from '@shared/common/list-view-component-base';
import { NzModalRef } from 'ng-zorro-antd';
import { Injector, Input } from '@angular/core';

export abstract class ModalListViewComponentBase<TModal, TList> extends ListViewComponentBase<TList> {
  title = '';
  modalRef: NzModalRef;

  @Input()
  modalInput: TModal;

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
