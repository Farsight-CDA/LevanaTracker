<script lang="ts">
	import * as echarts from 'echarts';
	import { onDestroy, onMount } from 'svelte';

	let _class: string;
	export { _class as class };

    let title:string

	let chart: HTMLElement | null;
	let resizeObserver: ResizeObserver;
	let echart: echarts.ECharts | null;

	onMount(() => {
		echart = echarts.init(chart);
		echart.setOption({
			title: {
				text: title
			},
			tooltip: {},
			xAxis: {
				data: ['shirt', 'cardigan', 'chiffon', 'pants', 'heels', 'socks']
			},
			yAxis: {},
			series: [
				{
					name: 'sales',
					type: 'bar',
					data: [5, 20, 36, 10, 10, 20]
				}
			]
		});
		resizeObserver = new ResizeObserver(() => {
			echart?.resize();
		});
		resizeObserver.observe(chart!);
	});

	onDestroy(() => {
		resizeObserver?.disconnect();
		echart?.clear();
	});
</script>

<div bind:this={chart} class="w-full {_class}"></div>
