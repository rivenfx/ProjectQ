<template>
  <div>{{ title }}</div>
  <config-provider :locale="getAntdLocale">
    <app-provider>
      <!-- <router-view /> -->
      <RouterView/>
    </app-provider>
  </config-provider>
</template>

<script lang="ts">
import { defineComponent, PropType } from 'vue';
import { ConfigProvider } from 'ant-design-vue';
import { AppProvider } from '/@/components/Application';
import { useTitle } from '/@/hooks/web/useTitle';
import { useLocale } from '/@/locales/useLocale';

export default defineComponent({
  components: {
    ConfigProvider,
    AppProvider
  },
  props: {
    title: {
      type: Object as PropType<string>,
      default: () => {
        return '标题';
      },
      validator: (input: string) => {
        return !!input;
      }
    }
  },
  mixins: [],
  mounted() {
  },
  watch: {
    title(val: string, oldVal: string) {
      console.log("title: " + val, oldVal);
    },
  },
  setup() {
    // Listening to page changes and dynamically changing site titles
    useTitle();

    // support Multi-language
    const { getAntdLocale } = useLocale();

    return {
      getAntdLocale
    }
  }
});
</script>

<style lang="less">
</style>
