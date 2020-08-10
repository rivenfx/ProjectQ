import { Injector, Input } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { NzModalRef } from 'ng-zorro-antd/modal';

export abstract class ModalComponentBase<T> extends AppComponentBase {

  private _modalInput: T;
  private _readonly: boolean;

  /** 标题前缀 */
  titlePrefix = this.l('label.create');
  /** 标题 */
  title = '';
  /** 模态框指针 */
  modalRef: NzModalRef;
  /** 编辑 */
  isEdit: boolean;

  /** 外部输入参数 */
  @Input()
  set modalInput(val: T) {
    this._modalInput = val;
    if (typeof (val) !== 'undefined') {
      this.isEdit = true;
    }
    if (!!val && !this.readonly) {
      this.titlePrefix = this.l('label.edit');
    }
  }

  /** 外部输入参数 */
  get modalInput(): T {
    return this._modalInput;
  }

  /** 只读 */
  set readonly(val: boolean) {
    this._readonly = val;
    if (val) {
      this.titlePrefix = this.l('label.readonly');
    }
  }

  /** 只读 */
  get readonly(): boolean {
    return this._readonly;
  }


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
