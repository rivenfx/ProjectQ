import {
  Component,
  forwardRef,
  Injector,
  Input,
  OnInit,
  SimpleChange,
  SimpleChanges,
  ViewChildren,
} from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ArrayService } from '@delon/util';
import { PermissionServiceProxy } from '@service-proxies';
import { ControlComponentBase } from '@shared/common';
import * as _ from 'loadsh';
import { NzSafeAny } from 'ng-zorro-antd/core/types/any';
import { NzFormatEmitEvent, NzTreeNode } from 'ng-zorro-antd/tree';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'permission-tree',
  templateUrl: './permission-tree.component.html',
  styleUrls: ['./permission-tree.component.less'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => PermissionTreeComponent),
    multi: true,
  }],
})
export class PermissionTreeComponent extends ControlComponentBase<string[]> implements OnInit {


  // @ViewChildren('tree') treeRef: NzTreeComponent;


  treeData: NzTreeNode[];

  treeSearchVal: string;

  /** 禁用模式下选中的映射 */
  disabledCheckedMap: { [P in string]: boolean } = {};

  constructor(
    injector: Injector,
    public arraySer: ArrayService,
    public permissionApiSer: PermissionServiceProxy,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.loading = true;
    this.permissionApiSer.getAllPermissionsWithTree()
      .pipe(finalize(() => {
        this.loading = false;
      }))
      .subscribe((res) => {

        (res as any[]).forEach(o => {
          o.displayName = this.l(o.name);
        });
        this.treeData = this.arraySer.arrToTreeNode(res, {
          idMapName: 'name',
          parentIdMapName: 'parent',
          titleMapName: 'displayName',
          cb: (item: NzSafeAny, parent: NzSafeAny, deep: number) => {

          },
        });

        if (Array.isArray(this.value)) {
          this.writeValue(_.clone(this.value));
        }
      });
  }

  onAfterViewInit(): void {
  }

  onDestroy(): void {
  }

  onInit(): void {
  }

  onInputChange(changes: { [P in keyof this]?: SimpleChange } & SimpleChanges) {

  }

  writeValue(obj: any): void {
    this.value = obj;

    if (this.disabled) {
      this.disabledCheckedMap = {};
      if (Array.isArray(this.value)) {
        for (const item of this.value) {
          this.disabledCheckedMap[item] = true;
        }
      }
    }
  }

  onNzCheckBoxChange(event: NzFormatEmitEvent) {
    const checkedKeys = this.arraySer.getKeysByTreeNode(event.checkedKeys);
    this.emitValueChange(checkedKeys);
  }
}

