<script lang="ts">
	export let seed: number;

	function generateTailwindColor(randomNumber: number): string {
		const colors = [
			'bg-blue-100',
			'bg-green-100',
			'bg-indigo-100',
			'bg-purple-100',
			'bg-pink-100',
			'bg-teal-100',
			'bg-orange-100',
			'bg-red-200',
			'bg-blue-200',
			'bg-green-200',
			'bg-yellow-200',
			'bg-indigo-200',
			'bg-purple-200',
			'bg-pink-200',
			'bg-gray-200',
			'bg-teal-200',
			'bg-orange-200'
		];

		const index = Math.floor(randomNumber * colors.length);
		return colors[index];
	}
	const tailwindColorClass = generateTailwindColor(seed);
</script>

<div class="w-full md:w-4/12">
	<div class="p-2">
		<div class="p-4 rounded-3xl {tailwindColorClass}">
			<slot />
		</div>
	</div>
</div>
