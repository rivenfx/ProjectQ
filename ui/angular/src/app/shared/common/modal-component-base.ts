import { Injector, Input, ViewChild, Directive } from '@angular/core';
import { NgForm } from '@angular/forms';
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
    if (typeof val !== 'undefined') {
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
      this.disableFormControls();
    }
  }

  /** 只读 */
  get readonly(): boolean {
    return this._readonly;
  }

  /** 页面表单 */
  @ViewChild('pageForm') pageForm: NgForm;

  constructor(injector: Injector) {
    super(injector);
    try {
      this.modalRef = injector.get(NzModalRef);
    } catch (e) {}
  }

  /** 关闭模态框-成功 */
  success(res: boolean | any = true) {
    if (this.modalRef) {
      this.modalRef.close(res);
    }
  }

  /** 关闭模态框-直接关闭 */
  close(res: boolean | any = false) {
    this.success(res);
  }

  /** 当页面状态为只读, 修改表单控件状态为禁用 */
  disableFormControls() {
    if (this.readonly) {
      // tslint:disable-next-line: forin
      for (const key in this.pageForm.controls) {
        this.pageForm.controls[key].disable();
      }
    }
  }

  /** 提交表单 */
  abstract submitForm(event?: any);
}
