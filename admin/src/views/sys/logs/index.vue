<template>
  <el-container class="h-full">
    <el-aside width="200px" class="h-full pr-2">
      <div
        ref="targetEl"
        class="bg-card border-border org-tree h-full w-full rounded-[.5vw] p-3"
      >
        <el-tree
          ref="categoryRef"
          node-key="label"
          default-expand-all
          :data="category"
          :default-expanded-keys="['系统日志']"
          current-node-key="系统日志"
          :highlight-current="true"
          :expand-on-click-node="false"
          @node-click="handleTypeClick"
        />
      </div>
    </el-aside>
    <el-container>
      <el-header
        class="bg-card border-border flex flex-wrap items-center justify-between gap-3 rounded-[.5vw]"
      >
        <div>
          <el-date-picker
            v-model="query.time"
            type="datetimerange"
            range-separator="至"
            value-format="YYYY-MM-DD hh:mm:ss"
            start-placeholder="开始日期"
            end-placeholder="结束日期"
          ></el-date-picker>
          <el-button
            icon="Search"
            type="primary"
            class="ml-2"
            @click="handleSearch"
          />
        </div>
        <div>
          <el-popconfirm title="确认删除选中的记录吗？" @confirm="handleDelete">
            <template #reference>
              <el-button
                icon="Delete"
                v-if="selectedRows.length > 0"
                plain
                type="danger"
              />
            </template>
          </el-popconfirm>
          <el-popconfirm title="确认要清空审计记录吗？" @confirm="handleClear">
            <template #reference>
              <el-button type="danger" plain>清空日志</el-button>
            </template>
          </el-popconfirm>
        </div>
      </el-header>
      <el-header
        style="height: 180px"
        class="bg-card border-border mt-2 flex flex-wrap items-center justify-between gap-3 rounded-[.5vw]"
      >
        <!-- <EchartsUI ref="chartRef" height="160px" /> -->
        <SoaChart ref="chartRef" :options="barOptions" :height="160" />
      </el-header>
      <el-main class="bg-card border-border mt-2 rounded-[.5vw]">
        <soaTable
          ref="tableRef"
          :columns="columns"
          :apiObj="fetchLogsPage"
          row-key="id"
          row-serial-number
          :customRow="customRow"
          @selection-change="selectionChange"
        >
          <template #bodyCell="{ text, column, record }">
            <template v-if="column.key === 'fullName'">
              <a>{{ text }}</a>
            </template>
            <template v-if="column.key === 'status'">
              <el-tag :type="record.status ? 'success' : 'danger'">
                {{ record.status ? "启用" : "禁用" }}
              </el-tag>
            </template>
            <template v-if="column.key === 'org'">
              {{ record?.organizeObj?.name }}
            </template>
          </template>
        </soaTable>
      </el-main>
    </el-container>
    <Drawer class="w-[500px]" destroyOnClose :footer="false" title="日志详情">
      <el-main style="padding: 0 20px">
        <el-descriptions
          :column="1"
          border
          size="small"
          class="log-desc"
          label-width="100px"
        >
          <el-descriptions-item label="请求接口">{{
            logData.address
          }}</el-descriptions-item>
          <el-descriptions-item label="请求方法">{{
            logData.method
          }}</el-descriptions-item>
          <el-descriptions-item label="日志级别">{{
            logData.levelName
          }}</el-descriptions-item>
          <el-descriptions-item label="日志时间">{{
            logData.operateTime
          }}</el-descriptions-item>
        </el-descriptions>
        <el-collapse v-model="activeNames" style="margin-top: 20px">
          <el-collapse-item title="参数" name="1">
            <el-alert
              :title="logData.parameters"
              :type="typeMap[logData.level]"
              :closable="false"
            ></el-alert>
          </el-collapse-item>
          <el-collapse-item title="详细" name="2">
            <div class="code">
              {{ logData.returnValue }}
            </div>
          </el-collapse-item>
        </el-collapse>
      </el-main>
    </Drawer>
  </el-container>
</template>
<script setup lang="ts">
import SoaChart from "@/component/soaChart/index.vue";
import { useSoaDrawer } from "@/component/soaDrawer/index.vue";
import { fetchLogsPage, fetchLogsChart, deleteLogs, clearLogs } from "@/api";
const barOptions = ref({});
const [Drawer, drawerApi] = useSoaDrawer();
const categoryRef = ref();
const selectedRows = ref([]);
const tableRef = ref();
const activeNames = ref(["1", "2"]);
const logData = ref({});
const typeMap = {
  info: "info",
  warn: "warning",
  error: "error",
};
const query = reactive({
  page: 1,
  key: "",
  time: [],
  times: "",
  level: 0,
  type: 0,
});
const category = [
  {
    label: "全部",
    type: "all",
    value: "0",
  },
  {
    label: "日志级别",
    children: [
      { label: "debug", type: "level", value: "1" },
      { label: "info", type: "level", value: "2" },
      { label: "warn", type: "level", value: "3" },
      { label: "error", type: "level", value: "4" },
      { label: "fatal", type: "level", value: "5" },
    ],
  },
  {
    label: "日志类型",
    children: [
      { label: "登录日志", type: "type", value: 1 },
      { label: "操作日志", type: "type", value: 2 },
    ],
  },
];

const columns = [
  // {
  //   title: 'id',
  //   dataIndex: 'id',
  //   key: 'id',
  //   resizable: true,
  //   fixed: true,
  //   width: 100,
  // },
  {
    title: "用户",
    dataIndex: "operateUser",
    key: "operateUser",
    width: 120,
    resizable: true,
    fixed: true,
  },
  {
    title: "级别",
    dataIndex: "levelName",
    key: "levelName",
    width: 100,
    resizable: true,
  },
  {
    title: "操作地址",
    dataIndex: "address",
    key: "address",
    minWidth: 200,
    resizable: true,
    ellipsis: true,
  },
  {
    title: "提交类型",
    dataIndex: "method",
    key: "method",
    width: 100,
    resizable: true,
    align: "center",
  },
  {
    title: "IP",
    dataIndex: "ip",
    key: "ip",
    width: 130,
    resizable: true,
  },
  {
    title: "执行时长(毫秒)",
    dataIndex: "executionDuration",
    key: "executionDuration",
    width: 120,
    resizable: true,
    align: "center",
  },
  {
    title: "执行模块",
    dataIndex: "module",
    key: "module",
    width: 200,
    resizable: true,
  },
  {
    title: "浏览器信息",
    dataIndex: "browser",
    key: "browser",
    width: 200,
    resizable: true,
  },
  {
    title: "操作时间",
    dataIndex: "operateTime",
    key: "operateTime",
    width: 180,
    resizable: true,
    fixed: "right",
  },
];
const initChart = async () => {
  const res = await fetchLogsChart();
  const typeArry: any = ["debug", "info", "warn", "error", "fatal"];
  const color: any = ["#409eff", "#e6a23c", "#78e853", "#f56c6c", "#9e87ff"];
  let seriesRes: any = [];
  res.count.forEach((m: any, index: number) => {
    seriesRes.push({
      barWidth: 15,
      color: color[index],
      data: m,
      type: "bar",
      name: typeArry[index],
      itemStyle: {
        borderRadius: [10, 10, 0, 0],
      },
    });
  });

  barOptions.value = {
    grid: {
      bottom: 0,
      containLabel: true,
      left: "1%",
      right: "1%",
      top: "2.5%",
    },
    series: seriesRes,
    tooltip: {
      axisPointer: {
        lineStyle: {
          color: "#4f69fd",
          width: 1,
        },
      },
      trigger: "axis",
    },
    xAxis: {
      data: res.time,
      type: "category",
    },
    yAxis: {
      splitNumber: 4,
      type: "value",
    },
  };
};
onMounted(() => {
  initChart();
});
const handleTypeClick = (node: any) => {
  query.page = 1;
  if (node.type == "all") {
    query.type = 0;
    query.level = 0;
  }
  if (node.type == "level") {
    query.level = node.value;
    query.type = 0;
  }
  if (node.type == "type") {
    query.type = node.value;
    query.level = 0;
  }
  tableRef.value.upData(query);
};
const selectionChange = (params: any) => {
  selectedRows.value = params.selectedRows;
};
const handleSearch = () => {
  if (query.time) {
    query.times = query.time.join("/");
  }
  tableRef.value.upData(query);
};
const handleDelete = async () => {
  const ids = selectedRows.value.map((m) => m.id);
  await deleteLogs(ids);
  ElMessage.success("删除成功！");
  handleSearch();
};
const handleClear = async () => {
  await clearLogs();
  ElMessage.success("删除成功！");
  handleSearch();
};
const customRow = (record: any) => {
  return {
    onClick: () => {
      logData.value = record;
      drawerApi.open();
    },
  };
};
</script>
<style scoped>
:deep(.org-tree .el-tree-node__content) {
  height: 36px;
  border-radius: 8px;
}
</style>
